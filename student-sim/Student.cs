using System;
using System.Collections.Generic;
using System.Text;

namespace SimStudentSu21 {
    class Student {
#region fields
        private string studentID;
        private uint age;
        private float numCreditHours;
        private DateTime dateOfBirth;
        private String name;
        private float gpa;
        private double energy;
        private double satiation;
        private double happiness;
        private double health;
        private double money;
#endregion

#region constructors
        public Student() {
            this.studentID = "000000";
            this.age = 0;
            this.numCreditHours = 0;
            this.dateOfBirth = new DateTime(0);
            this.name = "John Doe";
            this.gpa = 2.0f;
            this.energy = 50;
            this.satiation = 50;
            this.happiness = 50;
            this.health = 50;
            this.money = 50;
        }

        public Student(
                string studentID,
                uint age,
                float numCreditHours,
                DateTime dob,
                string name,
                float gpa,
                double energy,
                double satiation,
                double happiness,
                double health,
                double money
        ) {
            this.studentID = studentID;
            this.age = age;
            this.numCreditHours = numCreditHours;
            this.dateOfBirth = dob;
            this.name = name;
            this.gpa = gpa;
            this.energy = energy;
            this.satiation = satiation;
            this.happiness = happiness;
            this.health = health;
            this.money = money;
        }
#endregion

#region behaviors
        public void Study(int hours) {
            for (int elapsedTime = 0; elapsedTime < hours; elapsedTime++) {
                this.gpa += 0.02f;
                this.energy += 10;
                this.happiness -= 5;
                this.satiation -= 5;
            }
        }

        public void Sleep(int hours) {
            if (hours > 0) {
                if (hours > 1) {
                    Console.WriteLine($"{this.name} sleeps for {hours} hours.");
                }
                else if (hours == 1) {
                    Console.WriteLine($"{this.name} sleeps for an hour.");
                }
                for (int elapsedTime = 0; elapsedTime < hours; elapsedTime++) {
                    this.gpa += 0.02f;
                    this.energy += 10;
                    this.happiness -= 5;
                    this.satiation -= 5;
                }
            }
        }
#endregion

#region utilities
        public override string ToString() {
            return $"SID: {this.studentID} Name: {this.name} DOB: GPA: {this.gpa} Energy: {this.energy}";
        }
#endregion
    }
}
