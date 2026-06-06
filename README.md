# Match AI Quote

This is a game where you match AI generated meme translations with original quotes. You can play this by using [this link](https://romandykyi.github.io/MatchAIQuote/).

The translations was generated using llama3.2, and quotes are taken from [this dataset](https://www.kaggle.com/datasets/kieranpoc/quotes) by Kieran O'Connor ([CC BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/)).

## Licensing

This project uses a split-licensing model to separate the website's source code from the underlying data.

### Source Code (MIT License)
All website code, UI assets, and scripts are licensed under the **[MIT License](LICENSE)**. 

Feel free to use, modify, and distribute the code as you see fit.

### Data & Derived Content (CC BY-SA 4.0)

The datasets used in this project are adapted from the **WikiQuoteXL: Large-Scale Dataset of Quotes** by Kieran O'Connor, originally licensed under [CC BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/).

In compliance with the ShareAlike terms, all remixed and derived files listed below are also licensed under [CC BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/):

#### Original Quotes (`wwwroot/data/`)
  Files are prefixed with `quotes-`.
  *Modifications:* 
    - Removed select entries
    - Stripped out some bracketed text (inside `[]`)
    - Added custom categories.
#### Derived Content (subfolders inside `wwwroot/data/`)
  Files are prefixed with `trans-`.
  *Modifications:* 
    - Contains LLM-generated content directly based on the modified quotes.