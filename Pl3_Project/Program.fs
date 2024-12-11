open System
open System.Windows.Forms
open System.Drawing


// --- Define the Question type (Multiple-Choice or Written) 
type QuestionType = 
    | MultipleChoice
    | Written

type Question = {
    QuestionText: string
    Options: string[] option  // Some questions may have options (multiple-choice)
    CorrectAnswer: string
    QuestionType: QuestionType
}





// Sample quiz data (Create multiple-choice and written questions)
let questions = [
    { QuestionText = "What is the capital of France?"; Options = Some [| "Paris"; "London"; "Berlin"; "Rome" |]; CorrectAnswer = "Paris"; QuestionType = MultipleChoice }
    { QuestionText = "Which is the largest planet?"; Options = Some [| "Earth"; "Mars"; "Jupiter"; "Venus" |]; CorrectAnswer = "Jupiter"; QuestionType = MultipleChoice }
    { QuestionText = "What is 5 + 3?"; Options = None; CorrectAnswer = "8"; QuestionType = Written }
    { QuestionText = "Describe the water cycle."; Options = None; CorrectAnswer = "Evaporation, condensation, precipitation"; QuestionType = Written }
]

