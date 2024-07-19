import { Component, Input, Output } from '@angular/core';
import { MatCard } from '@angular/material/card';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  @Input() plan_id!: string;
  @Input() name!: string;
  @Input() description!: string;
  @Input() noCategory!: number;
  @Input() category!: string;
  @Input() image!: string;
  @Input() created_by!: string;

  //constructor (public card: MatCard){}

}
