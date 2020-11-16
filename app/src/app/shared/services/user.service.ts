import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user.model';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  addUser(user: User): Observable<boolean> {
   return this.http.post<boolean>(environment.url + 'user/adduser', user)
  }

  getUserExist(user: User) : Observable<number>{
    return this.http.post<number>(environment.url + 'user/getUserExist', user)

  }

  cookbook():Observable<string[]>{
    let userId=localStorage.getItem('currentUser');
    return this.http.get<string[]>(environment.url + 'user/getUserCookbook'+ userId)

  }
}
