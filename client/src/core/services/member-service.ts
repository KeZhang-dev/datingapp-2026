import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Member, Photo } from '../../types/member';
import id from '@angular/common/locales/id';


@Injectable({
  providedIn: 'root',
})
export class MemberService {
  private http = inject(HttpClient); //注入发送HTTP请求的服务
  private baseUrl = environment.apiUrl; //获取API的基础URL

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'members');
  }

  getMember(id: string) {
    return this.http.get<Member>(this.baseUrl + 'members/' + id);
  }

  getMemberPhotos(id: string) {
    return this.http.get<Photo[]>(this.baseUrl + 'members/' + id + '/photos');
  }
}
