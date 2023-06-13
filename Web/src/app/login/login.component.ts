import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LogInResult, LoginService } from './login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [LoginService]
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  returnUrl: string = '/';
  login_failed: boolean = false;


  private LoggedIn(): void { // route to return url
    console.log("User is logged in successful");
    this.router.navigateByUrl(this.returnUrl);

  }


  constructor(private auth_service: LoginService,
    private route: ActivatedRoute,
    private router: Router,) {
  }
  ngOnInit(): void {
    this.auth_service.getLoginEmitter().subscribe((res: LogInResult) => {
      console.log(res);
      if (res.loggedin)
        this.LoggedIn();
      else
        this.login_failed = true;

    });
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

  }

  Login() {
    this.auth_service.Login(this.username, this.password);
  }
}
