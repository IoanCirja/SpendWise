import { Component } from '@angular/core';

@Component({
  selector: 'app-our-team',
  templateUrl: './our-team.component.html',
  styleUrls: ['./our-team.component.scss']
})
export class OurTeamComponent {
  members = [
    { name: 'Barila Sabina Nadejda', imageUrl: "assets/Sabina.jpeg" ,description: 'Enthusiastic student passionate about fashion, reading and outdoor walks. I play piano and share positive energy. Apparently, I like the frontend more than the backend.'},
    { name: 'Cîrjă Ioan', imageUrl: "assets/Ioan.jpeg" ,description: 'Empathic thinker with a keen interest in the latest technologies, personal development, and a commitment to excellence. P.S. I also enjoy reading and campfires.'},
    { name: 'Cazamir Andrei', imageUrl: "assets/Andrei.jpeg" ,description: 'I am passionate about traveling, football and video games, spending my free time exploring new places, watching football matches and gaming.'},
    { name: 'Ghiuță Cristian-Daniel', imageUrl: "assets/Cristi.jpeg",description: 'I’m Cristi, a student with a passion for travel, cycling, football, and sports in general. Additionally, I have a deep love for animals, which adds another layer of joy to my life.' },
    { name: 'Huminiuc Simona', imageUrl: "assets/Simona.jpeg" ,description: 'I am passionate about painting nature, reading fiction books on my kindle and a dedicated gamer. I am a creative and organized individual who thrives as an early bird, making the most of each day.'}
  ];
}
