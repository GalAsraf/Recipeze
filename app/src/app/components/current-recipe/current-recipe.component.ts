import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { RecipeService } from 'src/app/shared/services/recipe.service';

@Component({
  selector: 'app-current-recipe',
  templateUrl: './current-recipe.component.html',
  styleUrls: ['./current-recipe.component.css']
})
export class CurrentRecipeComponent implements OnInit {
  inOrOut: boolean;
  recipe: Recipe = new Recipe();

  constructor(private route: ActivatedRoute, private recipeService: RecipeService) { }

  ngOnInit(): void {
    let r: Recipe;
    this.route.params.subscribe(
      p => {
        r = JSON.parse(p.recipe);
        this.recipe = r;
      }
    );

    this.recipeService.checkIfRecipeExist(this.recipe.RecipeName).subscribe(
      res => {
      this.inOrOut = res
      console.log(res)
      });
  }

  addRecipeToCookbook(recipe: Recipe) {
    //todo:
    // we have to make it that after he press "add", he would have a button "delete", that he wouldn't add it twice
   
  
    this.recipeService.addRecipeToCookbook(recipe).subscribe(
      res => console.log(res));
  }

  deleteRecipeFromCookbook(recipe: Recipe) {
    this.recipeService.deleteRecipeFromCookbook(recipe.RecipeName).subscribe(
      res => console.log(res));
  }
}
