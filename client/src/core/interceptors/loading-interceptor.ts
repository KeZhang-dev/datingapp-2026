import { HttpEvent, HttpInterceptorFn } from '@angular/common/http';
import { BusyService } from '../services/busy-service';
import { inject } from '@angular/core/primitives/di';
import { finalize } from 'rxjs/internal/operators/finalize';
import { delay, of, tap } from 'rxjs';


const cache = new Map<string, HttpEvent<unknown>>();

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const busyService = inject(BusyService); //注入BusyService服务，用于管理加载状态

  if (req.method === 'GET') {
    const cachedResponse = cache.get(req.url);
    if (cachedResponse) {
      return of(cachedResponse); //如果缓存中存在响应，则直接返回缓存的响应
    }
  }

  busyService.busy();
  return next(req).pipe(
    delay(500), //延迟500毫秒，避免闪烁
    tap(response => {
      cache.set(req.url, response); //将响应缓存起来，以便下次请求使用
    }),
    finalize(() => busyService.idle())
  );
};
