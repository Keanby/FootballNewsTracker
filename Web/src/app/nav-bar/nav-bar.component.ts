import { Component, OnChanges, SimpleChanges } from '@angular/core';
import { LoginService } from '../login/login.service';

@Component({
  selector: 'app-navigation-bar',
  template: `
    <p-menubar [model]="items"></p-menubar>
  `,
  providers: [LoginService]
})
export class NavigationBarComponent {


  items = [
    {
      label: 'News',
      icon: 'pi pi-home',
      routerLink: '/news'
    },
    {
      label: 'Watch later',
      icon: 'pi pi-info',
      routerLink: '/watch-later'
    },
    {
      label: 'Login',
      icon: 'pi pi-info',
      routerLink: '/login',
    },
    // Add more menu items as needed
  ];
}
