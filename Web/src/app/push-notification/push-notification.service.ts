import {  Injectable } from '@angular/core';
import { Observable } from '@apollo/client';
import { Apollo, gql } from 'apollo-angular';
import { map } from 'rxjs';
import { News } from '../dtos/news';




@Injectable({
  providedIn: 'root'
})
export class PushNotificationService {

  constructor(private apollo: Apollo) { }
  subcribeToNotifications(): Observable<News> | any {

    return this.apollo.subscribe<{ onNewsAdded: News }>({
      query: gql`
      subscription OnNewsAdded
      {
        onNewsAdded {
          id
          link
          title
          time
          addDateTime
        }
      }
      `,
    }).pipe(
      map((response) => response.data?.onNewsAdded)
    );
  }
}
