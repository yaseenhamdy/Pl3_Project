open System
open System.Windows.Forms
open System.Drawing


type QuestionType = 
    | MultipleChoice
    | Written

type Question = {
    QuestionText: string
    Options: string[] option  
    CorrectAnswer: string
    QuestionType: QuestionType
}





// Sample quiz data (Create multiple-choice and written questions)
let questions = [
    { QuestionText = "What is the capital of France?"; Options = Some [| "Paris"; "London"; "Berlin"; "Rome" |]; CorrectAnswer = "Paris"; QuestionType = MultipleChoice }
    { QuestionText = "Which is the largest planet?"; Options = Some [| "Earth"; "Mars"; "Jupiter"; "Venus" |]; CorrectAnswer = "Jupiter"; QuestionType = MultipleChoice }
    { QuestionText = "Who is best player in the world ?"; Options = Some [| "Messi 2022"; "Messi 2019"; "Messi 2009"; "All of the above" |]; CorrectAnswer = "All of the above"; QuestionType = MultipleChoice }
    { QuestionText = "What is 5 + 3?"; Options = None; CorrectAnswer = "8"; QuestionType = Written }
    { QuestionText = "Describe the water cycle."; Options = None; CorrectAnswer = "Evaporation, condensation, precipitation"; QuestionType = Written }
]






//  Store correct answers for comparison 
let checkAnswer (question: Question) (userAnswer: string) =
    userAnswer.Trim().ToLower() = question.CorrectAnswer.Trim().ToLower()



// Implement logic for tracking user scores 
let trackScore (questions: Question list) (userAnswers: string list) =
    List.fold2 (fun acc q answer -> if checkAnswer q answer then acc + 1 else acc) 0 questions userAnswers


// Create Windows Forms UI to display the questions 
// Create the form
let form = new Form(Text = "Quiz Application", Size = Size(500, 400), BackColor = Color.LightBlue)

let headerLabel = new Label(Location = Point(20, 10), Size = Size(450, 40), Text = "QuizzzzNow", Font = new Font("Arial", 20.0f, FontStyle.Bold), ForeColor = Color.DarkBlue)
form.Controls.Add(headerLabel)

let questionLabel = new Label(Location = Point(20, 60), Size = Size(450, 40), Font = new Font("Arial", 12.0f), ForeColor = Color.DarkBlue)
form.Controls.Add(questionLabel)
let mutable answerRadioButtons =   

   [ for i in 0 .. 3 do
        new RadioButton(Location = Point(20, 100 + i * 30), Size = Size(450, 30), Font = new Font("Arial", 10.0f)) ]
answerRadioButtons |> List.iter (fun rb -> form.Controls.Add(rb))

let writtenAnswerTextBox = new TextBox(Location = Point(20, 100), Size = Size(450, 30))
writtenAnswerTextBox.Visible <- false
form.Controls.Add(writtenAnswerTextBox)

let submitButton = new Button(Text = "Submit", Location = Point(20, 260), Size = Size(100, 40), BackColor = Color.LightGreen, Font = new Font("Arial", 10.0f))
form.Controls.Add(submitButton)

let resultLabel = new Label(Location = Point(20, 330), Size = Size(450, 40), Font = new Font("Arial", 12.0f), ForeColor = Color.Blue)
form.Controls.Add(resultLabel)





// Functionality for user input (answer selection)
let mutable currentQuestionIndex = 0
let mutable userAnswers = []

let createRadioButtons (numOptions: int) =
    [ for i in 0 .. numOptions - 1 do
        new RadioButton(Location = Point(20, 100 + i * 30), Size = Size(450, 30), Font = new Font("Arial", 10.0f)) ]

// Load the next question based on the question type
let loadQuestion (index: int) =
    if index < questions.Length then
        let question = questions.[index]
        questionLabel.Text <- question.QuestionText
        
        match question.QuestionType with
        | MultipleChoice ->
            answerRadioButtons |> List.iter (fun rb -> form.Controls.Remove(rb))
            
            answerRadioButtons <- createRadioButtons (question.Options.Value.Length)
            answerRadioButtons |> List.iteri (fun i rb -> 
                rb.Text <- question.Options.Value.[i]
                rb.Checked <- false
                rb.Visible <- true
                form.Controls.Add(rb)) 
            writtenAnswerTextBox.Visible <- false
        | Written ->
            answerRadioButtons |> List.iter (fun rb -> rb.Visible <- false)
            writtenAnswerTextBox.Visible <- true
    else
        questionLabel.Text <- "Quiz Finished!"
        let score = trackScore questions userAnswers
        resultLabel.Text <- sprintf "Your score is: %d / %d" score questions.Length
        answerRadioButtons |> List.iter (fun rb -> rb.Visible <- false)
        writtenAnswerTextBox.Visible <- false
        submitButton.Enabled <- false


//  Integrate all components and handle quiz flow 
submitButton.Click.Add(fun _ ->
    let selectedAnswer =
        if writtenAnswerTextBox.Visible then 
            writtenAnswerTextBox.Text  
        else
            answerRadioButtons 
            |> List.tryFind (fun rb -> rb.Checked) 
            |> Option.map (fun rb -> rb.Text)  
            |> Option.defaultValue ""  
    
    if selectedAnswer <> "" then
        userAnswers <- userAnswers @ [selectedAnswer]
        
        if writtenAnswerTextBox.Visible then
            writtenAnswerTextBox.Clear()

        currentQuestionIndex <- currentQuestionIndex + 1
        loadQuestion currentQuestionIndex
    else
        MessageBox.Show("Please select an answer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning) |> ignore
)


loadQuestion currentQuestionIndex

Application.Run(form)