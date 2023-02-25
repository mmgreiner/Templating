module MyScriban.Program

open System
open Chores

// https://github.com/scriban/scriban
open Scriban

let templateString = """
<ul>
{{- for person in Persons }}
    <li>
    <p>{{person.Name}} has the following tasks to do:</p>
    <ul>
        {{- if person.Chores | array.size == 0 }}
        Free afternoon
        {{ else }}
        {{- for task in person.Chores }}
            <li>{{ task.Name }} by {{task.DueDate | date.to_string "%Y-%m-%d"}} {{if task.Completed -}}, completed {{task.Completed.Value | date.to_string "%Y-%m-%d"}} {{end}}
            </li>
        {{- end }}
        {{- end }}
    </ul>
{{- end -}}
</ul>
"""

let template = Template.Parse(templateString)
let persons = {| Persons = Chores.Data.persons |}


// need to prevent standard ruby naming https://github.com/scriban/scriban/blob/master/doc/runtime.md#member-renamer
let result = template.Render(persons, fun m -> m.Name)

printfn "%s" result
