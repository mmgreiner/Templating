module MyHandlebars.Program

open System
open Chores.Data
open HandlebarsDotNet

let templateString = """
<ul>
{{#each Persons}}
    <li>
    <p>{{this.Name}} has the following tasks to do:</p>
    <ul>
        {{#each this.Chores}}
        <li>{{this.Name}} by {{date_format this.DueDate}}{{#if this.Completed}}, completed {{date_format this.Completed.Value}} {{/if}}
        </li>
        {{else}}
        <li>Free afternoon</li>
        {{/each}}
    </ul>
{{/each}}
</ul>
"""


Handlebars.RegisterHelper ("date_format", fun writer (context: Context) (parameters: Arguments) ->
        let dt =
            match parameters |> Seq.head with
            | :? DateTime as d -> d.ToString("yyyy-MM-dd")
            | _ -> sprintf "Type of %s is %A, not DateTime" (context.Value.ToString()) (context.Value.GetType())
        writer.WriteSafeString(dt)
    )

let template = Handlebars.Compile(templateString)
let data = {| Persons = Chores.Data.persons |}
let result = template.Invoke(data)

printfn "Handlebars"
printfn "%s" result 