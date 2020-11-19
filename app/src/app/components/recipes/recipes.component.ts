import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { CategoryService } from 'src/app/shared/services/category.service';

@Component({
  selector: 'app-recipes',
  templateUrl: './recipes.component.html',
  styleUrls: ['./recipes.component.css']
})
export class RecipesComponent implements OnInit {
  categoryToSearchBy: string;
  treatSens: string;

  recipes: Recipe[] = [];
  constructor(private route: ActivatedRoute, private categoryService: CategoryService, private router: Router) { }

  ngOnInit(): void {
    let allergies: number[];


    this.route.params.subscribe(

      //, p.treatSens cause i want to send also if to treat thesensitive or not
      //  p => this.categoryService.googleSearch(p.search).subscribe(
      p => {
        allergies = JSON.parse(p.whatChecked)
        this.categoryService.googleSearch(p.search, JSON.parse(p.whatChecked)).subscribe(
          res => {
            this.recipes = res;
            console.log(res)
          },
          err => { console.error(err) }
        )
      }
    );


  }

  showRecipe(recipe: Recipe) {
    this.router.navigate(['current-recipe',JSON.stringify(recipe)]);
  }
}
