import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { NewsListComponent } from './news-list/news-list.component';
import { WatchLaterComponent } from './watch-later/watch-later.component';
import { NewsElementComponent } from './news-list/news-element/news-element.component';
import { RouterModule } from '@angular/router';
import { MessagesModule } from 'primeng/messages';

import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PaginatorModule } from 'primeng/paginator';
import { GraphQLModule } from './graphql.module';
import { MenubarModule } from 'primeng/menubar';
import { NavigationBarComponent } from './nav-bar/nav-bar.component';
import { AuthGuardGuard } from './auth-guard.guard';
import { PushNotificationComponent } from './push-notification/push-notification.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NewsListComponent,
    WatchLaterComponent,
    NewsElementComponent,
    NavigationBarComponent,
    PushNotificationComponent,
  ],
  imports: [
    BrowserModule, HttpClientModule,
    TableModule, ButtonModule, BrowserAnimationsModule, PaginatorModule,
    MenubarModule, MessagesModule,
    RouterModule.forRoot([
      { path: '', component: NewsListComponent, pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'news', component: NewsListComponent },
      { path: 'watch-later', component: WatchLaterComponent, canActivate: [AuthGuardGuard] },

    ]),
    GraphQLModule,
  ],
  providers: [AuthGuardGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
