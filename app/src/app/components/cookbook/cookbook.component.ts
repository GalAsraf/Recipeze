import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { RecipeService } from 'src/app/shared/services/recipe.service';

@Component({
  selector: 'app-cookbook',
  templateUrl: './cookbook.component.html',
  styleUrls: ['./cookbook.component.css']
})
export class CookbookComponent implements OnInit {
  cookbookList: Recipe[] = [];
  title = "angular-text-search-hightlight";
  searchText='';
  characters=[
    'chocolate chip cookies',
    'tuna pasta salad',
    'sweet potato fries',
    'avocado salad',
    'vanilla cake',
    'chocolate mousse'
  ]
  constructor(private route: ActivatedRoute, private recipeService: RecipeService) { }

  ngOnInit(): void {
    this.recipeService.getUserCookbook().subscribe(
      res => {
        this.cookbookList = res;
        console.log(res)
      },
      err => { console.error(err) }
    );
  }


  removeRecipeFromCookbook(recipe:Recipe){
    this.recipeService.deleteRecipeFromCookbook(recipe).subscribe(
      res => console.log(res));
  }


}



