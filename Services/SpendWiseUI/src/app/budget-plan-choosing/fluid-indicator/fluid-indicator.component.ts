import { Component, Input, OnChanges, SimpleChanges, AfterViewInit, ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-fluid-indicator',
  templateUrl: './fluid-indicator.component.html',
  styleUrls: ['./fluid-indicator.component.scss']
})
export class FluidIndicatorComponent implements OnChanges, AfterViewInit {
  @Input() value: number = 50;
  @ViewChild('progressElement', { static: false }) progressElement!: ElementRef<HTMLDivElement>;

  ngAfterViewInit(): void {
    this.updateProgress(); // Ensure the progress is updated when the view initializes
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['value']) {
      this.updateProgress();
    }
  }

  updateProgress(): void {
    console.log('Updating progress with value:', this.value);

    // Add a short delay to allow the view to fully initialize
    setTimeout(() => {
      if (this.progressElement) {
        const progressElement = this.progressElement.nativeElement;
        const displayValue = this.value >= 100 ? -1 : Math.round(this.value); // Set to -1 if value is 100 or more
        progressElement.style.setProperty('--progress-value', `${displayValue}`);
        progressElement.dataset.value = `${displayValue}`;
        if (this.value >= 100) {
          progressElement.classList.add('progress--upper-half-value');
        } else if (this.value > 50) {
          progressElement.classList.add('progress--upper-half-value');
          progressElement.classList.remove('progress--upper-half-value');
        } else {
          progressElement.classList.remove('progress--upper-half-value');
        }
      } else {
        console.warn('Progress element not found.');
      }
    }, 0);
  }
}
