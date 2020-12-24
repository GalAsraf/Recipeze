import { Component, OnInit } from '@angular/core';
import { Allergy } from 'src/app/shared/models/allergy.model';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { AllergyService } from 'src/app/shared/services/allergy.service';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { RecipeService } from 'src/app/shared/services/recipe.service';
import { healthArticles } from 'src/app/shared/models/healthArticles.modal';
import { HealthArticlesService } from 'src/app/shared/services/health-articles.service';
import Speech from 'speak-tts'


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
  sentEmail1: string;
  sentEmail2: string;
  sentEmail3: string;

  constructor(private allergiesService: AllergyService, private modalService: NgbModal,
    private recipeService: RecipeService, private healthArticlesService: HealthArticlesService) {
    this.places = ['fadeInLeft', 'fadeInUp', 'fadeInRight'];

    // const speech = new Speech()
    // speech.init().then((data) => {
    //   // The "data" object contains the list of available voices and the voice synthesis params
    //   console.log("Speech is ready, voices are available", data)
    // }).catch(e => {
    //   console.error("An error occured while initializing : ", e)
    // })

    // Speech.setLanguage('en-US');
    // Speech.setVoice('Fiona');
    // Speech.setRate(1);
    // Speech.setVolume(1);
    // Speech.setPitch(1);


    // Speech.init({
    //   'volume': 1,
    //   'lang': 'en-GB',
    //   'rate': 1,
    //   'pitch': 1,
    //   'voice': 'Google UK English Male',
    //   'splitSentences': true,
    //   'listeners': {
    //     'onvoiceschanged': (voices) => {
    //       console.log("Event voiceschanged", voices)
    //     }
    //   }
    // })

    // speech.speak({
    //   text: 'Hello, how are you today ?',
    // }).then(() => {
    //   console.log("Success !")
    // }).catch(e => {
    //   console.error("An error occurred :", e)
    // })
  }


  ngOnInit(): void {
    this.sentEmail1 = "https://mail.google.com/mail/u/0/?view=cm&fs=1&su=";
    this.sentEmail2 = "&body=";
    this.sentEmail3 = "&tf=1";
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
    this.email(this.currentRecipe.RecipeName, this.currentRecipe.Ingredients, this.currentRecipe.Method);
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

  email(subject: string, ingredients: string[], method: string[]) {
    this.sentEmail1 = this.sentEmail1.concat(subject);
    this.sentEmail1 = this.sentEmail1.concat(this.sentEmail2);
    ingredients.forEach(a => this.sentEmail1 = this.sentEmail1.concat(a), this.sentEmail1 = this.sentEmail1.concat("\n"));
    method.forEach(a => this.sentEmail1 = this.sentEmail1.concat(a), this.sentEmail1 = this.sentEmail1.concat("\n"));
    this.sentEmail1 = this.sentEmail1.concat(this.sentEmail3);
    console.log(this.sentEmail1)
  }


  print(): void {
    let printContents, popupWin;
    printContents = document.getElementById('print-section') as HTMLInputElement;
    //innerHtml doesn't work!
    //printContents= printContents.innerHTML;
    popupWin = window.open('', '_blank', 'top=0,left=0,height=100%,width=auto');
    popupWin.document.open();
    popupWin.document.write(`
      <html>
        <head>
          <title>Print tab</title>
          <style>
          //........Customized style.......
          </style>
        </head>
    <body onload="window.print();window.close()">${printContents}</body>
      </html>`
    );
    popupWin.document.close();
  }

}
