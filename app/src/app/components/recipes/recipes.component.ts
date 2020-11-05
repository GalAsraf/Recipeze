import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { CategoryService } from 'src/app/shared/services/category.service';

@Component({
  selector: 'app-recipes',
  templateUrl: './recipes.component.html',
  styleUrls: ['./recipes.component.css']
})
export class RecipesComponent implements OnInit {

  recipes:Recipe[]=[];
  constructor(private route:ActivatedRoute,private categoryService:CategoryService) { }

  ngOnInit(): void {
      
    this.route.params.subscribe(
      p=> this.categoryService.googleSearch(p.search).subscribe(
        res => { this.recipes=res;
        console.log(res) },
        err => { console.error(err) }
      )
    );
   
  }

}
