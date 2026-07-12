import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  busyRequestCount = signal(0); //定义一个计数器，用于记录当前正在进行的请求数量

  busy() {
    this.busyRequestCount.update(current => current + 1); //当有新的请求开始时，计数器加1
  }

  idle() {
    this.busyRequestCount.update(current => Math.max(0, current - 1)); //当请求完成时，计数器减1
  }
}
