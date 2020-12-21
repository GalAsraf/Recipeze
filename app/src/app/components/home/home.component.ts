import { Component, OnInit } from '@angular/core';
import { Allergy } from 'src/app/shared/models/allergy.model';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { AllergyService } from 'src/app/shared/services/allergy.service';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { RecipeService } from 'src/app/shared/services/recipe.service';
import { healthArticles } from 'src/app/shared/models/healthArticles.modal';
import { HealthArticlesService } from 'src/app/shared/services/health-articles.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  login: boolean = true;
  substitutes: string[];
  allergiesForUser: Allergy[] = [];
  places: string[];
  lastRecipes: Recipe[]
  closeResult: string;
  currentRecipe: Recipe;
  inOrOut: boolean;
  articles: healthArticles[];


  constructor(private allergiesService: AllergyService, private modalService: NgbModal,
    private recipeService: RecipeService, private healthArticlesService: HealthArticlesService) {
    this.places = ['fadeInLeft', 'fadeInUp', 'fadeInRight'];
  }

  ngOnInit(): void {
    this.lastRecipes = JSON.parse(localStorage.getItem('last-search'));
    
    this.healthArticlesService.getRandomArticles().subscribe(
      res => {
        this.articles = res
      });

    this.allergiesService.getCurrentUserAllergies().subscribe(
      res => {
        this.allergiesForUser = res;
      });

    if (localStorage.getItem('currentUser') != null) {
      this.login == true;
      this.allergiesService.getSubstitutes().subscribe(
        res => {
          //res can be null if there is no sensitivities
          this.substitutes = res;
        });
    }
    else
      this.login == false;
  }


  open(content, recipe) {
    this.currentRecipe = recipe;
    this.recipeService.checkIfRecipeExist(this.currentRecipe).subscribe(
      res => {
        this.inOrOut = res
        console.log(res)
        this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
          this.closeResult = `Closed with: ${result}`;
        }, (reason) => {
          this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        });
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



  addRecipeToCookbook(recipe: Recipe) {
    this.inOrOut = true;
    this.recipeService.addRecipeToCookbook(recipe).subscribe(
      res => console.log(res)),
      err => this.inOrOut = false
  }
}
