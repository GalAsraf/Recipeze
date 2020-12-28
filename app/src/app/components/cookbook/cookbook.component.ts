import { Component, OnInit } from '@angular/core';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { RecipeService } from 'src/app/shared/services/recipe.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';



@Component({
  selector: 'app-cookbook',
  templateUrl: './cookbook.component.html',
  styleUrls: ['./cookbook.component.css']
})
export class CookbookComponent implements OnInit {
  cookbookList: Recipe[] = [];
  cookbookToShow: Recipe[] = [];
  title = "angular-text-search-hightlight";
  searchText = '';
  currentRecipe: Recipe;
  closeResult: string;
  sentEmail1: string;
  sentEmail2: string;
  sentEmail3: string;


  constructor(private modalService: NgbModal, private route: ActivatedRoute, private recipeService: RecipeService,
    private router: Router) { }
  // public dialogService: DialogService

  ngOnInit(): void {
    this.loadRecipes();
    this.sentEmail1 = "https://mail.google.com/mail/u/0/?view=cm&fs=1&su=";
    this.sentEmail2 = "&body=";
    this.sentEmail3 = "&tf=1";

  }

  loadRecipes() {
    debugger
    this.recipeService.getUserCookbook().subscribe(
      res => {
        this.cookbookList = res;
        console.log(res)
        this.showRecipeList();
      },
      err => { console.error(err) }
    );

  }

  showRecipeList() {
    this.cookbookToShow = this.cookbookList;
    // this.cookbookList.forEach(r => {
    //   this.cookbookToShow.push(r.RecipeName)
    // });
  }


  removeRecipeFromCookbook(recipe: Recipe) {
    this.recipeService.deleteRecipeFromCookbook(recipe).subscribe(
      res => {
        console.log(res);
       
      }
    );
  }

  

  showRecipe(recipe: Recipe) {
    this.router.navigate(['current-recipe', JSON.stringify(recipe)]);


    //   const ref = this.dialogService.open(CurrentRecipeComponent, {
    //     data: {currentRecipe:recipe},
    //     header: recipe.RecipeName,
    //     width: '70%'
    // });
  }


  open(content, recipe) {
    this.currentRecipe = recipe;
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  deleteRecipeFromCookbook(recipe: Recipe) {
    this.recipeService.deleteRecipeFromCookbook(recipe).subscribe(
      res => {
        console.log(res);
        this.loadRecipes(); 
      });
  }

  
  email(subject: string, ingredients: string[], method: string[]) {
    this.sentEmail1.concat(subject);
    this.sentEmail1.concat(this.sentEmail2);
    ingredients.forEach(a=>this.sentEmail1.concat(a));
    method.forEach(a=>this.sentEmail1.concat(a));
    this.sentEmail1.concat(this.sentEmail3);
  }

}



