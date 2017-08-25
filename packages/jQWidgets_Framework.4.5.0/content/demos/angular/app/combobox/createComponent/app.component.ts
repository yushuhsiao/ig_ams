/// <reference path="../../../jqwidgets-ts/jqwidgets.d.ts" /> 
import { Component, ViewChild, AfterViewInit } from '@angular/core';

import { jqxComboBoxComponent } from '../../../../../jqwidgets-ts/angular_jqxcombobox';

@Component({
    selector: 'my-app',
    template: `<jqxComboBox #comboBoxReference [auto-create]='false'></jqxComboBox>`
}) 

export class AppComponent implements AfterViewInit
{ 
    @ViewChild('comboBoxReference') myComboBox: jqxComboBoxComponent;

    ngAfterViewInit(): void
    {
        this.myComboBox.createComponent(this.settings);
    }   

    settings: jqwidgets.ComboBoxOptions =
    {
        source: this.generateHTML(),
        selectedIndex: 0,
        width: '250',
        height: '25px'
    }

    generateHTML()
    {
        let source = new Array();

        for (let i = 0; i < 10; i++)
        {
            let movie = 'avatar.png';
            let title = 'Avatar';
            let year = 2009;
            switch (i)
            {
                case 1:
                    movie = 'endgame.png';
                    title = 'End Game';
                    year = 2006;
                    break;
                case 2:
                    movie = 'priest.png';
                    title = 'Priest';
                    year = 2011;
                    break;
                case 3:
                    movie = 'unknown.png';
                    title = 'Unknown';
                    year = 2011;
                    break;
                case 4:
                    movie = 'unstoppable.png';
                    title = 'Unstoppable';
                    year = 2010;
                    break;
                case 5:
                    movie = 'twilight.png';
                    title = 'Twilight';
                    year = 2008;
                    break;
                case 6:
                    movie = 'kungfupanda.png';
                    title = 'Kung Fu Panda';
                    year = 2008;
                    break;
                case 7:
                    movie = 'knockout.png';
                    title = 'Knockout';
                    year = 2011
                    break;
                case 8:
                    movie = 'theplane.png';
                    title = 'The Plane';
                    year = 2010;
                    break;
                case 9:
                    movie = 'bigdaddy.png';
                    title = 'Big Daddy';
                    year = 1999;
                    break;
            }
            let html = "<div style='padding: 0px; margin: 0px; height: 95px; float: left;'><img width='60'" +
                "style='float: left; margin-top: 4px; margin-right: 15px;' src='../../../../images/" + movie
                + "'/><div style='margin-top: 10px; font-size: 13px;'>" + "<b>Title</b><div>" + title +
                "</div><div style='margin-top: 10px;'><b>Year</b><div>" + year.toString() + "</div></div></div>";
            source[i] = { html: html, title: title };
        }
        return source;
    }
}
