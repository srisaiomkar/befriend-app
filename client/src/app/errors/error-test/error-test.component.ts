import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-error-test',
  templateUrl: './error-test.component.html',
  styleUrls: ['./error-test.component.css']
})
export class ErrorTestComponent implements OnInit {
  baseUrl = 'https://localhost:5001/api/'
  constructor(private http : HttpClient) { }

  ngOnInit(): void {
  }

  test400Error(){
    this.http.get(this.baseUrl +'errortest/bad-request').subscribe(response =>{
      console.log(response);
    }, error =>{
      console.log(error);
    })
  }

  test401Error(){
    this.http.get(this.baseUrl +'errortest/auth').subscribe(response =>{
      console.log(response);
    }, error =>{
      console.log(error);
    })
  }

  test404Error(){
    this.http.get(this.baseUrl +'errortest/not-found').subscribe(response =>{
      console.log(response);
    }, error =>{
      console.log(error);
    })
  }

  test500Error(){
    this.http.get(this.baseUrl +'errortest/server-error').subscribe(response =>{
      console.log(response);
    }, error =>{
      console.log(error);
    })
  }

  test400ValidationError(){
    this.http.post(this.baseUrl + 'account/register',{}).subscribe(response =>{
      console.log(response);
    }, error =>{
      console.log(error);
    })
  }


}
