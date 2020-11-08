import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Recipe } from 'src/app/shared/models/recipe.model';
import { CategoryService } from 'src/app/shared/services/category.service';

@Component({
  selector: 'app-recipes',
  templateUrl: './recipes.component.html',
  styleUrls: ['./recipes.component.css']
})
export class RecipesComponent implements OnInit {
  categoryToSearchBy: string;
  treatSens: string;

  recipes: Recipe[] = [];
  constructor(private route: ActivatedRoute, private categoryService: CategoryService) { }

  ngOnInit(): void {
    this.categoryToSearchBy = this.route.snapshot.paramMap.get('select');
    this.treatSens = this.route.snapshot.paramMap.get('check');

    this.route.params.subscribe(
      //, p.treatSens cause i want to send also if to treat thesensitive or not
    //  p => this.categoryService.googleSearch(p.search).subscribe(

        p => this.categoryService.googleSearch(p.search, p.whatChecked).subscribe(
          res => {
          this.recipes = res;
          console.log(res)
        },
        err => { console.error(err) }
      )
    );

  }

}
