import { isNull } from '@angular/compiler/src/output/output_ast';
import { unsupported } from '@angular/compiler/src/render3/view/util';
import { Component } from '@angular/core';
import { User } from './models/identity/User';
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(public accountService: AccountService){}

  ngOnInit():void{
    this.setCurrentUser();
  }

  setCurrentUser(): void{
    let user: User;
    if (localStorage.getItem('user')){
        user = JSON.parse(localStorage.getItem('user') ??'{}');
    }
    else{
      user = new User;
    }
    if (user)
      this.accountService.setCurrentUser(user);
  }
}
