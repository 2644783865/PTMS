import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  logo = require('../assets/logo.png');
  navigation = [
    { link: 'home', label: 'Главная' }
  ];

  isAuthenticated$: Observable<boolean>;

  ngOnInit() {
    this.isAuthenticated$ = new Observable<boolean>();
  }

}
