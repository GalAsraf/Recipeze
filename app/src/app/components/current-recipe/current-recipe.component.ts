import { Component, OnInit } from '@angular/core';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { RecipeService } from 'src/app/shared/services/recipe.service';

@Component({
  selector: 'app-current-recipe',
  templateUrl: './current-recipe.component.html',
  styleUrls: ['./current-recipe.component.css']
})
export class CurrentRecipeComponent implements OnInit {

  constructor(private recipeService: RecipeService) { }

  ngOnInit(): void {
  }

  addRecipeToCookbook(recipe: Recipe) {
    //todo:
    // we have to make it that after he press "add", he would have a button "delete", that he wouldn't add it twice
    this.recipeService.addRecipeToCookbook(recipe).subscribe(
      res => console.log(res));
  }
}
