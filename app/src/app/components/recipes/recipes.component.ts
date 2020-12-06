import { Component, OnInit, NgModule, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { CategoryService } from 'src/app/shared/services/category.service';
import { DialogService } from 'primeng/dynamicdialog';
import { CurrentRecipeComponent } from '../current-recipe/current-recipe.component';
import { BrowserModule } from '@angular/platform-browser';
import { GecoDialog, GecoDialogModule } from 'angular-dynamic-dialog'; // <-- import the module
import { AppComponent } from '../../app.component';

@NgModule({
  imports: [
    BrowserModule
    ],
  declarations: [CurrentRecipeComponent],
  bootstrap: [RecipesComponent],
  entryComponents: [CurrentRecipeComponent]
})

@Component({
  selector: 'app-recipes',
  templateUrl: './recipes.component.html',
  styleUrls: ['./recipes.component.css'],
  providers: [DialogService]

})
export class RecipesComponent implements OnInit {
  categoryToSearchBy: string;
  treatSens: string;
  recipes: Recipe[];
  constructor(private route: ActivatedRoute, private categoryService: CategoryService, private router: Router, public dialogService: DialogService, private modal: GecoDialog) {

   }

  //The first parameter is the component to be rendered in the modal's content
  //The second parameter is the modal's configuration



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
    //this.router.navigate(['current-recipe',JSON.stringify(recipe)]);

    let m = this.modal.openDialog(CurrentRecipeComponent, {
      //Inject data
      data: { recipe },
      //Options: 'bootstrap', 'none'
      useStyles: 'none'
    });

    //Event when closing the modal
    m.onClosedModal().subscribe(() => {
      alert('Closed modal!!!');
    });
    //Event when opening the modal
    m.onOpenModal().subscribe(null, null, () => {
      alert('open modal');
    });
    //   const ref = this.dialogService.open(CurrentRecipeComponent, {
    //     data: {currentRecipe:recipe},
    //     header: recipe.RecipeName,
    //     width: '70%'
    // });
  }
}


