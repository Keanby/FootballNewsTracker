import { Component, OnInit } from '@angular/core';
import { News } from '../dtos/news';
import { WatchLaterService } from '../watch-later/watch-later.service';
import { NewsService } from './news-service.service';

@Component({
  selector: 'app-news-list',
  templateUrl: './news-list.component.html',
  styleUrls: ['./news-list.component.css'],
  providers: [NewsService]
})
export class NewsListComponent implements OnInit {

  constructor(
    private newsService: NewsService,
    private watchService :  WatchLaterService) {
    this.newsService.getAllNews().subscribe(res => {
      this.news.push(...res);
      //values.sort((one, two) => (one > two ? -1 : 1));
      this.news.sort((a, b) => ( a.id > b.id ? -1 : 1 ));
      console.log(res);
    });
  }

  ngOnInit(): void {
  }
  news: News[] = []



  addWatchLater(element: News) {
    this.watchService.addWatchLater(element.id);
  }


}
