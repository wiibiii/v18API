import { Component } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  collapsed = true;

  toggleCollapsed(){
    this.collapsed = !this.collapsed;
  }
}
