import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { EditableMember, Member, Photo } from '../../types/member';
import id from '@angular/common/locales/id';
import { tap } from 'rxjs/internal/operators/tap';


@Injectable({
  providedIn: 'root',
})
export class MemberService {
  private http = inject(HttpClient); //注入发送HTTP请求的服务
  private baseUrl = environment.apiUrl; //获取API的基础URL
  editMode = signal(false); //定义一个布尔值，用于表示是否处于编辑模式
  member = signal<Member | null>(null); //定义一个信号，用于存储当前成员的数据

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'members');
  }

  getMember(id: string) {
    return this.http.get<Member>(this.baseUrl + 'members/' + id).pipe(
      tap(member => {
        this.member.set(member);
      })
    )
  }

  getMemberPhotos(id: string) {
    return this.http.get<Photo[]>(this.baseUrl + 'members/' + id + '/photos');
  }

  updateMember(member: EditableMember) {
    return this.http.put(this.baseUrl + 'members', member);
  }

  uploadPhoto(file: File) {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<Photo>(this.baseUrl + 'members/add-photo', formData);
  }

  setMainPhoto(photo: Photo) {
    return this.http.put(this.baseUrl + 'members/set-main-photo/' + photo.id, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'members/delete-photo/' + photoId, {});
  }
}
