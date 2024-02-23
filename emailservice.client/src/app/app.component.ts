import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ApiService } from './services/api.service';
import { ActivatedRoute, ActivatedRouteSnapshot, Router, UrlSegment } from '@angular/router';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public forecasts: WeatherForecast[] = [];
  public backgroundImageUrl: string = '';

  constructor(private http: HttpClient,public apiService:ApiService,private route: ActivatedRoute,private router: Router) {}


  ngOnInit(): void {
    // console.log(this.router);
    this.route.url.subscribe(url => {
      console.log("Current route:", url);
    });
   
  }
}
