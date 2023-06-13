import { Injectable } from '@angular/core';
import { Apollo, gql } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { News } from '../dtos/news';
import { WatchLater } from '../dtos/watch-later';

@Injectable({
  providedIn: 'root'
})
export class WatchLaterService {

  constructor(private apollo: Apollo) { }


  addWatchLater(id: number): void {
    debugger
    this.apollo.mutate<{ addWatchLater: WatchLater }>({
      mutation: gql`
      mutation AddWatchLater($addId : Int!)
      {
        addWatchLater(input: {newsId: $addId}) {
          userId
          newsId
        }
      }
      `,
      variables: {
        addId: id
      }
    }).subscribe(res => { }, err => { console.error(err) });
  }

  removeWatchLater(id: number) {
    
  }

  getWatchLater(): Observable<WatchLater[]> {
    return this.apollo.query<{ userWatchLater: WatchLater[] }>({
      query: gql`
      query GetMyWatchLaterList
      {
        userWatchLater{
          userId
          newsId
        }
      }
      `
    }).pipe(
      map((response) => response.data.userWatchLater)
    );
  }
}
