package
{
	import flash.display.Bitmap;
	import flash.utils.Dictionary;
	import flash.utils.ByteArray;
	
	import starling.textures.Texture;
	import starling.textures.TextureAtlas;
	
	import starling.text.TextField;
	
	

	public class Assets
	{
	//[Embed(source="../media/graphics/bgWelcome.jpg")]
	//public static const BgWelcome:Class;
		[Embed(source="/media/font/BERNHC_0.ttf", embedAsCFF="false", fontName = "BERNHC")]
private static const BERNHC:Class;
		
		[Embed(source="/media/font/方正大黑简体.ttf", embedAsCFF="false", fontName = "方正大黑简体")]
private static const newFont:Class;
		
		[Embed(source="/media/graphics/ph-table-Bg.jpg")]
		public static const BgLayer:Class;
		
		private static var gameTextures:Dictionary = new Dictionary();
		private static var gameTextureAtlas:TextureAtlas;
		
			private static var cardTextures:Dictionary = new Dictionary();
		private static var cardTextureAtlas:TextureAtlas;
		
		[Embed(source="/media/graphics/Assets.png")]
		public static const AtlasTextureGame:Class;
		
		[Embed(source="/media/graphics/Assets.xml", mimeType="application/octet-stream")]
		public static const AtlasXmlGame:Class;
		
		[Embed(source="/media/graphics/cards.png")]
		public static const AtlasTextureCards:Class;
		
		[Embed(source="/media/graphics/cards.xml", mimeType="application/octet-stream")]
		public static const AtlasXmlCards:Class;
		
		public static function getAtlas():TextureAtlas
		{
			var texture: Texture;
			var xml:XML;
			if (gameTextureAtlas == null)
			{
				texture = getTexture("AtlasTextureGame");
				xml = XML(new AtlasXmlGame());
				gameTextureAtlas = new TextureAtlas(texture, xml);
			}
		
			return gameTextureAtlas;
		}
		
		public static function getAtlas1():TextureAtlas
		{
			var texture: Texture;
			var xml:XML;
			if (cardTextureAtlas == null){
				texture = getTexture("AtlasTextureCards");
				 xml = XML(new AtlasXmlCards());
				cardTextureAtlas = new TextureAtlas(texture, xml);
			}
			return cardTextureAtlas;
		}

		public static function getTexture(name:String):Texture
		{
			if (gameTextures[name] == undefined)
			{
				var bitmap:Bitmap = new Assets[name]();
				gameTextures[name] = Texture.fromBitmap(bitmap);
			}
			return gameTextures[name];
		}
	}
}