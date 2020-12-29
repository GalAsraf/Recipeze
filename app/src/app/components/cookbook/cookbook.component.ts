import { Component, OnInit } from '@angular/core';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { RecipeService } from 'src/app/shared/services/recipe.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import Speech from 'speak-tts';
import { DOCUMENT } from '@angular/common';


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
  html = '';
  result = '';
  speech: any;
  speechData: any;
  stringToRead:string="";


  constructor(private modalService: NgbModal, private route: ActivatedRoute, private recipeService: RecipeService,
    private router: Router) { 

      this.speech = new Speech() // will throw an exception if not browser supported
    if (this.speech.hasBrowserSupport()) { // returns a boolean
      console.log("speech synthesis supported")
      this.speech.init({
        'volume': 1,
        'lang': 'en-GB',
        'rate': 1,
        'pitch': 1,
        'voice': 'Google UK English Male',
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
    this.sentEmail1 = "https://mail.google.com/mail/u/0/?view=cm&fs=1&su=";
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      this.sentEmail1 = "https://mail.google.com/mail/u/0/?view=cm&fs=1&su=";
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
    this.sentEmail1 = this.sentEmail1.concat(subject);
    this.sentEmail1 = this.sentEmail1.concat(this.sentEmail2);
    this.sentEmail1 = this.sentEmail1.concat("%0A"+"ingredients"+"%0A");
    ingredients.forEach(a =>
      { 
         this.sentEmail1 = this.sentEmail1.concat(a+"%0A")}
      );
      this.sentEmail1 = this.sentEmail1.concat("%0A"+"instruction"+"%0A");
    method.forEach(a => this.sentEmail1 = this.sentEmail1.concat(a+"%0A"));
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
    this.creatingString(recipeName, ingredient, method);
    console.log("sts=art")
    this.speech.speak({
      text: this.stringToRead,
      //text: "How are you gal? how you doing?",
    }).then(() => {
      console.log("Success !")
    }).catch(e => {
      console.error("An error occurred :", e)
    })
  }
  creatingString(recipeName, ingredient, method){
    this.stringToRead = this.stringToRead.concat(recipeName+". ");
    this.stringToRead = this.stringToRead.concat("ingredients:");
    ingredient.forEach(a => this.stringToRead = this.stringToRead.concat(a+". "));
    this.stringToRead = this.stringToRead.concat("instructions:");
    method.forEach(a => this.stringToRead = this.stringToRead.concat(a+". "));
    this.stringToRead = this.stringToRead.concat("enjoy your meal!!")

    }

  pause() {
    this.speech.pause();
  }
  resume() {
    this.speech.resume();
  }

  setLanguage(i) {
    console.log(i);
    console.log(this.speechData.voices[i].lang + this.speechData.voices[i].name);
    this.speech.setLanguage(this.speechData.voices[i].lang);
    this.speech.setVoice(this.speechData.voices[i].name);
  }
}



