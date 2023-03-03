import { NgModule } from '@angular/core';
import { NumbersDirective } from './number.directive';

@NgModule({
  imports: [],
  declarations: [NumbersDirective],
  exports: [NumbersDirective]
})
export class NumbersDirectivesModule { }