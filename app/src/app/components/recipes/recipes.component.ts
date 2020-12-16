import { Component, ViewEncapsulation, OnInit, NgModule, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { CategoryService } from 'src/app/shared/services/category.service';
import { DialogService } from 'primeng/dynamicdialog';
import { CurrentRecipeComponent } from '../current-recipe/current-recipe.component';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';



@Component({
  selector: 'app-recipes',
  templateUrl: './recipes.component.html',
  styleUrls: ['./recipes.component.css'],
  encapsulation: ViewEncapsulation.None,
  providers: [DialogService],
  styles: [`
    .dark-modal .modal-content {
      background-color: #292b2c;
      color: white;
    }
    .dark-modal .close {
      color: white;
    }
    .light-blue-backdrop {
      background-color: #5cb3fd;
    }
  `]
})

export class RecipesComponent implements OnInit {
  categoryToSearchBy: string;
  treatSens: string;
  recipes: Recipe[];
  closeResult: string;
  currentRecipe:Recipe;

  constructor(private route: ActivatedRoute, private categoryService: CategoryService,
    private router: Router, public dialogService: DialogService,
    private modalService: NgbModal) {
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

  // openScrollableContent(content) {
  //   this.modalService.open(content, { scrollable: true });
  // }


  showRecipe(recipe: Recipe) {
    this.router.navigate(['current-recipe', JSON.stringify(recipe)]);
    const ref = this.dialogService.open(CurrentRecipeComponent, {
      data: { currentRecipe: recipe },
      header: recipe.RecipeName,
      width: '70%'
    });
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
}



