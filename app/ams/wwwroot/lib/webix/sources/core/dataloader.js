import {ajax} from "../load/ajax";
import {bind, delay, extend, toFunctor} from "../webix/helpers";
import {proto} from "../ui/core";

import {dp} from "../load/dataprocessor";
import proxy from "../load/proxy";
import promise from "../thirdparty/promiz";

import DataStore from "../core/datastore";
import AtomDataLoader from "../core/atomdataloader";


/*
	Behavior:DataLoader - load data in the component
	
	@export
		load
		parse
*/
const DataLoader =proto({
	$init:function(config){
		//prepare data store
		config = config || "";

		this._feed_last = {};
		this._data_generation = 1;

		this.data = new DataStore();
		this.data.attachEvent("onClearAll",bind(this._call_onclearall,this));
		this.data.attachEvent("onServerConfig", bind(this._call_on_config, this));
		this.attachEvent("onDestruct", this._call_onclearall);

		this.data.feed = this._feed;
		this.data.owner = config.id;
	},
	_feed:function(from,count,callback,defer){
		//allow only single request at same time
		if (this._load_count){
			defer = promise.defer();
			this._load_count=[from,count,callback,defer];	//save last ignored request
			return defer;
		}
		else
			this._load_count=true;
		this._feed_last.from = from;
		this._feed_last.count = count;
		return this._feed_common.call(this, from, count, callback, false, false, defer);
	},
	_feed_common:function(from, count, callback, url, details, defer){
		var state = null;
		url = url || this.data.url;

		if (from<0) from = 0;

		if(!details)
			details = { start: from, count:count };

		if(this.count())
			details["continue"] = "true";

		if (this.getState)
			state = this.getState();

		// proxy
		if (url && typeof url != "string"){
			if (state){
				if (state.sort)
					details.sort = state.sort;
				if (state.filter)
					details.filter = state.filter;
			}
			return this.load(url, 0, details).then(
				data => this._feed_on_load(data, callback, defer),
				() => this._feed_callback()
			);
		} else { // GET
			url = url+((url.indexOf("?")==-1)?"?":"&");

			var params = [];
			for(var d in details){
				params.push(d+"="+details[d]);
			}
			if (state){
				if (state.sort)
					params.push("sort["+state.sort.id+"]="+encodeURIComponent(state.sort.dir));
				if (state.filter)
					for (var key in state.filter){
						var filterValue = state.filter[key];
						if(typeof filterValue == "object")
							filterValue = ajax().stringify(filterValue); //server daterangefilter
						params.push("filter["+key+"]="+encodeURIComponent(filterValue));
					}
			}

			url += params.join("&");
			if (this._feed_last.url !== url){
				this._feed_last.url = url;
				return this.load(url).then(
					data => this._feed_on_load(data, callback, defer),
					() => this._feed_callback()
				);
			} else {
				this._load_count = false;
			}
		}
	},
	_feed_on_load:function(data, callback, defer){
		delay(()=> this._feed_callback(), "", "", 100);
		if(callback)
			ajax.$callback(this, callback, data);
		if(defer)
			defer.resolve(data);
		return data;
	},
	_feed_callback:function(){
		//after loading check if we have some ignored requests
		var temp = this._load_count;
		this._load_count = false;
		if (typeof temp =="object")
			this.data.feed.apply(this, temp);	//load last ignored request
	},
	//loads data from external URL
	load:function(url){
		url = proxy.$parse(url);
		var ajax = AtomDataLoader.load.apply(this, arguments);

		//prepare data feed for dyn. loading
		if (!this.data.url)
			this.data.url = url;

		return ajax;
	},
	//load next set of data rows
	loadNext:function(count, start, callback, url, now){
		var config = this._settings;
		if (config.datathrottle && !now){
			if (this._throttle_request)
				window.clearTimeout(this._throttle_request);

			let defer = promise.defer();
			this._throttle_request = delay(function(){
				defer.resolve(this.loadNext(count, start, callback, url, true));
			},this, 0, config.datathrottle);
			return defer;
		}

		if (!start && start !== 0) start = this.count();
		if (!count)
			count = config.datafetch || this.count();

		this.data.url = this.data.url || url;
		if (this.callEvent("onDataRequest", [start,count,callback,url]) && this.data.url){
			let result = this.data.feed.call(this, start, count, callback);
			if(result && result.then)
				return result;
			else //loading was blocked due to same url
				return promise.reject("Attempt to load data with the same url");
		}
	},
	_maybe_loading_already:function(count, from){
		var last = this._feed_last;
		if(this._load_count && last.url){
			if (last.from<=from && (last.count+last.from >= count + from )) return true;
		}
		return false;
	},
	removeMissed_setter:function(value){
		return (this.data._removeMissed = value);
	},
	//init of dataprocessor delayed after all settings processing
	//because it need to be the last in the event processing chain
	//to get valid validation state
	_init_dataprocessor:function(){
		var url = this._settings.save;

		if (url === true)
			url = this._settings.save = this._settings.url;

		var obj = { master: this };
		
		if (url && url.url)
			extend(obj, url);
		else
			obj.url = url;

		dp(obj);
	},
	save_setter:function(value){
		if (value)
			this.$ready.push(this._init_dataprocessor);

		return value;
	},
	scheme_setter:function(value){
		this.data.scheme(value);
	},
	dataFeed_setter:function(value){
		value = proxy.$parse(value);

		this.data.attachEvent("onBeforeFilter", bind(function(text, filtervalue){
			//complex filtering, can't be routed to dataFeed
			if (typeof text == "function") return true;

			//we have dataFeed and some text
			if (this._settings.dataFeed && (text || filtervalue)){
				text = text || "id";
				if (filtervalue && typeof filtervalue == "object")
					filtervalue = filtervalue.id;

				this.clearAll();
				var url = this._settings.dataFeed;

				//url data feed
				if(typeof url =="string"){
					var urldata = "filter["+text+"]="+encodeURIComponent(filtervalue);
					this.load(url+(url.indexOf("?")<0?"?":"&")+urldata, this._settings.datatype);
				}
				//js data feed
				else{
					var filter = {};
					filter[text] = filtervalue;
					if (typeof url == "function"){
						url.call(this, filtervalue, filter);
					}
					else if (url.$proxy) {
						if (url.load){
							url.load(this, { filter: filter }).then(
								data => this._onLoad(data),
								x => this._onLoadError(x)
							);
						}
					}
				}
				return false;
			}
		},this));
		return value;
	},
	_call_onready:function(){
		if (this._settings.ready && !this._ready_was_used){
			var code = toFunctor(this._settings.ready, this.$scope);
			if (code)
				delay(code, this, arguments);
			if (this.callEvent)
				delay(this.callEvent, this, ["onReady", []]);
			this._ready_was_used = true;
		}
	},
	_call_onclearall:function(soft){
		this._data_generation++;
		if (!soft){
			this._load_count = false;
			this._feed_last = {};
			this.waitData = promise.defer();
		}
	},
	_call_on_config:function(config){
		this._parseSeetingColl(config);
	}
},AtomDataLoader);



export default DataLoader;