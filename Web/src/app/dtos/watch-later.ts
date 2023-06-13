import { News } from "./news";

export interface WatchLater {
  userId: number,
  newsId: number,
}

export interface UserWatchLater {
  userId: number,
  news: [News]
}
