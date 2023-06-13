import { EventEmitter, Injectable, Output } from '@angular/core';
import { gql } from '@apollo/client';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Apollo } from 'apollo-angular';
import { User, UserLogin } from '../dtos/user';


export class LogInResult {
  constructor(public loggedin: boolean) { }
}

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private jwtHelper: JwtHelperService = new JwtHelperService();
  @Output() fireIsLoggedIn: EventEmitter<LogInResult> = new EventEmitter<LogInResult>();

  constructor(private apollo: Apollo) { }

  IsLogin(): boolean {
    return !this.jwtHelper.isTokenExpired(localStorage.getItem('token'));
  }

  Login(username: string, password: string): void {
    this.apollo.query<{ loginUser: UserLogin }>({
      query: gql`
      query LoginUser($user: UserLoginInput!)
      {
        loginUser(user: $user) {
          username
          login
          jwtToken
        }
      }
      `,
      variables: {
        user: {
          username: username,
          password: password
        }
      }
    }).subscribe(res => {
      let loginResult: UserLogin = res.data.loginUser;
      localStorage.removeItem('token');
      localStorage.setItem('token', loginResult.jwtToken);
      this.fireIsLoggedIn.emit(new LogInResult(this.IsLogin()))
    })
  }

  Logout(): void {
    localStorage.removeItem('token');
    this.fireIsLoggedIn.emit({ loggedin: false });
  }

  getLoginEmitter() { return this.fireIsLoggedIn; }
}
