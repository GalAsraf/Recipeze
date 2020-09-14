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

  getUserExist() : Observable<boolean>{
    return this.http.get<boolean>(environment.url + 'user/adduser')

  }
}
