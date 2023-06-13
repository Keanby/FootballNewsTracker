import { Component, OnInit } from '@angular/core';
import { Message } from 'primeng/api/message';
import { PushNotificationService } from './push-notification.service';




@Component({
  selector: 'app-push-notification',
  templateUrl: './push-notification.component.html',
  styleUrls: ['./push-notification.component.css'],
  providers: [PushNotificationService]
})
export class PushNotificationComponent implements OnInit {
  public messages: Message[] = [];

  constructor(private pushService: PushNotificationService) {
    this.messages.push({ severity: 'info', closable: true, detail: "details", summary: "new update" });
    this.messages.push({ severity: 'info', closable: true, detail: "details", summary: "new update" });
  }
  ngOnInit() {
    this.pushService.subcribeToNotifications().subscribe((res: any) => {
      console.log(res);
      var msg: string = `Title: ${res.title}, time: ${res.time}`;
      this.messages = this.messages.concat([{ severity: 'info', closable: true, detail: msg, summary: "Watch updates:" }]);
    })
  }

}
