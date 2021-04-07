import { Component, OnInit } from '@angular/core';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { RecipeService } from 'src/app/shared/services/recipe.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import Speech from 'speak-tts';
import { DOCUMENT } from '@angular/common';
import { ViewChild, ElementRef } from '@angular/core';


@Component({
  selector: 'app-cookbook',
  templateUrl: './cookbook.component.html',
  styleUrls: ['./cookbook.component.css'],
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
export class CookbookComponent implements OnInit {
  cookbookList: Recipe[] = [];
  cookbookToShow: Recipe[] = [];
  deleted : boolean = false;
  inOrOut: boolean = true;
  added : boolean = false;
  title = "angular-text-search-hightlight";
  searchText = '';
  currentRecipe: Recipe;
  closeResult: string;
  sentEmail1: string;
  sentEmail2: string;
  sentEmail3: string;
  html = '';
  result = '';
  speech: any;
  speechData: any;
  stringToRead:string="";
  mark: boolean = false;
  bold: boolean = false;
  regular: boolean = true;
  contentttt: any;
  stop:boolean=false;
  fontSize = 18;
  @ViewChild('para', { static: true }) para: ElementRef;

  constructor(private modalService: NgbModal, private route: ActivatedRoute, private recipeService: RecipeService,
    private router: Router) { 
      this.mark = false;
      this.bold = false;
      this.regular = true;
      this.stop = false;
      this.loadRecipes();
      this.speechConstractor();
    }

  ngOnInit(): void {
    this.loadRecipes();
    this.sentEmail1 = "https://mail.google.com/mail/u/0/?view=cm&fs=1&su=";
    this.sentEmail2 = "&body=";
    this.sentEmail3 = "&tf=1";

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

  loadRecipes() {
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


  // removeRecipeFromCookbook(recipe: Recipe) {
  //   this.recipeService.deleteRecipeFromCookbook(recipe).subscribe(
  //     res => {
  //       console.log(res);
  //       this.loadRecipes(); 
  //     }
  //   );
  // }
 

  showRecipe(recipe: Recipe) {
    this.router.navigate(['current-recipe', JSON.stringify(recipe)]);
  }


  open(content, recipe) {
    this.added = false;
    this.inOrOut = true;
    this.deleted=false;
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

  deleteRecipeFromCookbook(recipe: Recipe) {
    this.inOrOut = false;
    this.recipeService.deleteRecipeFromCookbook(recipe).subscribe(
      res => {
        this.added= true;
        console.log(res);
        this.loadRecipes(); 
      });
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



  openWheelchair(wc){
    this.modalService.open(wc, {ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason2(reason)}`;
    });
  }

  private getDismissReason2(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }
}
