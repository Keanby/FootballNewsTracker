import { Injectable } from '@angular/core';
import { Apollo, gql } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { News } from '../dtos/news';

@Injectable({
  providedIn: 'root'
})
export class NewsService {

  constructor(private apollo: Apollo) { }


  getAllNews(): Observable<News[]> {
    return this.apollo.query<{ news: News[] }>({
      query: gql`
        query GetNews
        {
          news
          {
            id
            link
            title
            time
            addDateTime
          }
        }
      `
    }).pipe(
      map((response) => response.data.news)
    );
  }


  getNewsByIds(ids: number[]): Observable<News[]> {
    return this.apollo.query<{ news: News[] }>({
      query: gql`
        query GetNewsByIds($newsIds : [Int])
        {
            news(where: {id: {in: $newsIds}})
          {
            id
            link
            title
            time
            addDateTime
          }
        }
      `,
      variables: {
        newsIds: ids
      }  
    }).pipe(
      map((response) => response.data.news)
    );
  }

}
