import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { Allergy } from 'src/app/shared/models/allergy.model';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { AllergyService } from 'src/app/shared/services/allergy.service';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { RecipeService } from 'src/app/shared/services/recipe.service';
import { healthArticles } from 'src/app/shared/models/healthArticles.modal';
import { HealthArticlesService } from 'src/app/shared/services/health-articles.service';
import Speech from 'speak-tts';
import { DOCUMENT } from '@angular/common';


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
  html = '';
  result = '';
  speech: any;
  speechData: any;
  stringToRead: string = "";
  mark: boolean = false;
  bold: boolean = false;
  regular: boolean = true;
  contentttt: any;
  stop: boolean = false;
  fontSize = 14;
  @ViewChild('para', { static: true }) para: ElementRef;


  constructor(private allergiesService: AllergyService, private modalService: NgbModal,
    private recipeService: RecipeService, private healthArticlesService: HealthArticlesService,
    @Inject(DOCUMENT) document) {
    this.places = ['fadeInLeft', 'fadeInUp', 'fadeInRight'];
    this.mark = false;
    this.bold = false;
    this.regular = true;
    this.stop = false;
    this.speechConstractor();
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

  speechConstractor() {
    this.speech = new Speech() // will throw an exception if not browser supported
    if (this.speech.hasBrowserSupport()) { // returns a boolean
      console.log("speech synthesis supported")
      this.speech.init({
        'volume': 1,
        'lang': 'en-US',
        'rate': 0.7,
        'pitch': 1,
        'voice': 'Google US English',
        'splitSentences': true,
        'listeners': {
          'onvoiceschanged': (voices) => {
            console.log("Event voiceschanged", voices)
          }
        }
      }).then((data) => {
        // The "data" object contains the list of available voices and the voice synthesis params
        console.log("Speech is ready, voices are available", data)
        this.speechData = data;
        data.voices.forEach(voice => {
          console.log(voice.name + " " + voice.lang)
        });
      }).catch(e => {
        console.error("An error occured while initializing : ", e)
      })
    }
  }



  addRecipeToCookbook(recipe: Recipe) {
    this.inOrOut = true;
    this.recipeService.addRecipeToCookbook(recipe).subscribe(
      res => console.log(res)),
      err => this.inOrOut = false
  }

  open(content, recipe) {
    this.mark = false;
    this.bold = false;
    this.regular = true;
    this.contentttt = content;
    this.currentRecipe = recipe;
    this.creatingString(this.currentRecipe.RecipeName, this.currentRecipe.Ingredients, this.currentRecipe.Method);
    this.sentEmail1 = "https://mail.google.com/mail/u/0/?view=cm&fs=1&su=";
    this.email(this.currentRecipe.RecipeName, this.currentRecipe.Ingredients, this.currentRecipe.Method);
    this.recipeService.checkIfRecipeExist(this.currentRecipe).subscribe(
      res => {
        this.inOrOut = res
        console.log(res)
        this.modalService.open(content, { size: 'lg', ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
          this.closeResult = `Closed with: ${result}`;
          this.end();
        }, (reason) => {
          this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
          this.end();
          this.sentEmail1 = "https://mail.google.com/mail/u/0/?view=cm&fs=1&su=";
        });
      });
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      this.end();
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      this.end();
      return 'by clicking on a backdrop';
    } else {
      this.end();
      return `with: ${reason}`;
    }
  }

  email(subject: string, ingredients: string[], method: string[]) {
    this.sentEmail1 = this.sentEmail1.concat(subject);
    this.sentEmail1 = this.sentEmail1.concat(this.sentEmail2);
    this.sentEmail1 = this.sentEmail1.concat("%0A" + "ingredients" + "%0A");
    ingredients.forEach(a => {
      this.sentEmail1 = this.sentEmail1.concat(a + "%0A")
    }
    );
    this.sentEmail1 = this.sentEmail1.concat("%0A" + "instruction" + "%0A");
    method.forEach(a => this.sentEmail1 = this.sentEmail1.concat(a + "%0A"));
    this.sentEmail1 = this.sentEmail1.concat(this.sentEmail3);
    console.log(this.sentEmail1)
  }


  print(recipeName): void {
    let printContents, popupWin;
    printContents = document.getElementById('printElement').innerHTML;
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
    <body onload="window.print();window.close()">${recipeName} ${printContents}</body>
      </html>`
    );
    popupWin.document.close();
  }



  start(recipeName, ingredient, method) {
    console.log("sts=art start talking")
    this.speech.speak({
      text: this.stringToRead,
    }).then(() => {
      console.log("Success !")
    }).catch(e => {
      console.error("An error occurred :", e)
    })
    //this.stringToRead="";
  }

  creatingString(recipeName: string, ingredient: string[], method: string[]) {
    this.stringToRead = "";
    this.stringToRead = this.stringToRead.concat(recipeName);
    this.stringToRead = this.stringToRead.concat(".....ingredients...............");
    ingredient.forEach(a => this.stringToRead = this.stringToRead.concat(a + ". "));
    this.stringToRead = this.stringToRead.concat(".......instructions..........");
    method.forEach(a => this.stringToRead = this.stringToRead.concat(a + ". "));
    this.stringToRead = this.stringToRead.concat("......enjoy your meal!!")
  }

  pause() {
    this.speech.pause();
  }
  resume() {
    this.speech.resume();
  }
  end() {
    console.log("canceling");
    this.speech.cancel();
    this.speechConstractor();
  }

  setLanguage(i) {
    console.log(i);
    console.log(this.speechData.voices[i].lang + this.speechData.voices[i].name);
    this.speech.setLanguage(this.speechData.voices[i].lang);
    this.speech.setVoice(this.speechData.voices[i].name);
  }

  changeFont(operator) {
    operator === '+' ? this.fontSize++ : this.fontSize--;
    // console.log(operator);
    // console.log(this.contentttt);    
    // console.log(this.contentttt.nativeElement);    
    (this.para.nativeElement as HTMLParagraphElement).style.fontSize = `${this.fontSize}px`;
    //.(this.para.nativeElement as HTMLParagraphElement)
  }

  marking() {
    if (this.mark == true) {
      this.mark = false;
      this.regular = true;
      this.bold = false;
    }
    else {
      this.mark = true;
      this.bold = false;
      this.regular = false;
    }
  }

  bolding() {
    if (this.bold == true) {
      this.bold = false;
      this.regular = true;
      this.mark = false;
    }
    else
      this.bold = true;
    this.mark = false;
    this.regular = false;
  }



  openWheelchair(wc) {
    this.modalService.open(wc, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

}
