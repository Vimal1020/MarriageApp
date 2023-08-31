import { JsonPipe } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/Member';

const httpOptions = {
  headers: new HttpHeaders({
    Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))
  })
}

// var header = {
//   headers: new HttpHeaders()
//     .set('Authorization',  `Basic ${btoa(localStorage.getItem('user'))}`)
// }

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { 
    
  }
 
  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'members', httpOptions);
  }

   getMember(username){
    return this.http.get<Member>(this.baseUrl + 'members/' + username, httpOptions);
   }
}
