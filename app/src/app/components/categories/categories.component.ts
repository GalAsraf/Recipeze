import { Component, OnInit } from '@angular/core';
import { Category } from 'src/app/shared/models/category.model';
import { CategoryService } from 'src/app/shared/services/category.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {
categories:Category[]=[]
  constructor(private categoryService:CategoryService) { }

  ngOnInit(): void {
this.categoryService.getAllCategories().subscribe(
  res=>this.categories=res
)
  }

}
