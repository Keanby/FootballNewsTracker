import { Component, OnInit } from '@angular/core';
import { News } from '../dtos/news';
import { NewsService } from '../news-list/news-service.service';
import { WatchLaterService } from './watch-later.service';

@Component({
  selector: 'app-watch-later',
  templateUrl: './watch-later.component.html',
  styleUrls: ['./watch-later.component.css'],
  providers: [WatchLaterService]
})
export class WatchLaterComponent implements OnInit {

  constructor(
    private watchLaterService: WatchLaterService,
    private newsService: NewsService) {
    //this.newsService.getAllNews().subscribe(res => {
    //  this.watchLaterNews.push(...res);
    //  console.log(res);
    //});
  }

  ngOnInit(): void {
    this.watchLaterService.getWatchLater().subscribe(watchLaterElements => {
      
      let getIds = watchLaterElements.map(wl => wl.newsId);


      this.newsService.getNewsByIds(getIds).subscribe(news => {
        news.forEach(val => { this.watchLaterNews.push(val); })
      })

    })
  }
  watchLaterNews: News[] = []



  removeElement(element: News) {
    console.log(element);
  }

}
