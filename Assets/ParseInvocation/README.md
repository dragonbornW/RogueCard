# ParseInvocaion

将表达式转化为调用树

## 文法

```

ident_path
    : @ident
    | ident_path '.' @ident
    ;

literal
    : @integer
    | @integer _ @ident
    | @float
    | @float _ @ident
    | "true"
    | "false"
    | @string
    ;

call_params
    : '(' ')'
    | '[' ']'
    | '{' '}' ^ !no_brace
    | '(' arg_list<> ')'
    | '[' arg_list<> ']'
    | '{' arg_list<> '}' ^ !no_brace
    | '(' arg_list<> ',' ')'
    | '[' arg_list<> ',' ']'
    | '{' arg_list<> ',' '}' ^ !no_brace
    ;

arg_list
    : expr
    | arg_list ',' expr
    ;

primary
    : literal<>
    | ident_path<> 
    | ident_path<> call_params
    | '(' comma_expr<> ')'
    | '[' comma_expr<> ']'
    | '{' comma_expr<> '}' ^ !no_brace
    ;

unary
    : primary
    | '!' unary
    | '-' unary
    ;

multipicative_0
    : unary
    | multipicative_0 '*' unary
    | multipicative_0 '/' unary
    | multipicative_0 '%' unary
    ;

multipicative
    : multipicative_0
    | multipicative_0 ! '*='
    | multipicative_0 ! '/='
    | multipicative_0 ! '%='
    ;

additive_0
    : multipicative
    | additive_0 '+' multipicative
    | additive_0 '-' multipicative
    ;

additive
    : additive_0
    | additive_0 ! '+='
    | additive_0 ! '-='
    ;

comparison
    : additive
    | additive ! '='
    | additive '==' additive
    | additive '!=' additive
    | additive '>' additive
    | additive '<' additive
    | additive '>=' additive
    | additive '<=' additive
    ;

logical_and
    : comparison
    | logical_and '&' comparison
    ;

logical_or
    : logical_and
    | logical_or '|' logical_and
    ;

assignment
    : logical_or
    | logical_or '=' assignment
    | logical_or '+=' assignment
    | logical_or '-=' assignment
    | logical_or '*=' assignment
    | logical_or '/=' assignment
    | logical_or '%=' assignment
    ;

if_expr
    : "if" expr<no_brace> '{' comma_expr '}'
    | "if" expr<no_brace> '{' comma_expr '}' else_expr
    ;

else_expr
    : "else" '{' comma_expr '}'
    | "else" if_expr
    ;

expr
    : assignment
    | if_expr<>
    ;

#comma_expr
    : expr
    | comma_expr ',' expr
    ;

```