import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { RecipeService } from 'src/app/shared/services/recipe.service';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-current-recipe',
  templateUrl: './current-recipe.component.html',
  styleUrls: ['./current-recipe.component.css']
})
export class CurrentRecipeComponent implements OnInit {
  inOrOut: boolean;
  recipe: Recipe = new Recipe();
  recipeToSend:Recipe;

  constructor(private route: ActivatedRoute, private recipeService: RecipeService, public ref: DynamicDialogRef, public config: DynamicDialogConfig) { }

  ngOnInit(): void {
    let r: Recipe;
    this.recipe = this.config.data.currentRecipe;
    // this.recipeToSend.RecipeName=this.config.data.currentRecipe.RecipeName;
    // this.recipeToSend.PictureSource = this.recipe.PictureSource;

    console.log(this.recipe)
    this.recipeService.checkIfRecipeExist(this.recipe).subscribe(
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
    this.recipeService.deleteRecipeFromCookbook(recipe).subscribe(
      res => console.log(res));
  }
}
