Feature: Formatting a Feature

Scenario: Description should be Formatted as Markdown

  Given I have this feature description
        """
        In order to see the description as nice HTML
        As a Pickles user
        I want to see the descriptions written in markdown rendered as HTML

        Introduction
        ============

        This feature should have some markdown elements that get displayed properly

        Context
        -------

        > I really like blockquotes to describe
        > to describe text

        I also enjoy using lists as well, here are the reasons

        - Lists are easy to read
        - Lists make my life easier

        I also enjoy ordering things

        1. This is the first reason
        2. This is the second reason
        """
	When I generate the documentation
	Then the result should be
       """
       <div id="feature">
         <h1>a feature</h1>
         <div class="description">
           <p>In order to see the description as nice HTML
       As a Pickles user
       I want to see the descriptions written in markdown rendered as HTML</p>
           <h1>Introduction</h1>
           <p>This feature should have some markdown elements that get displayed properly</p>
           <h2>Context</h2>
           <blockquote>
             <p>I really like blockquotes to describe
       to describe text</p>
           </blockquote>
           <p>I also enjoy using lists as well, here are the reasons</p>
           <ul>
             <li>Lists are easy to read</li>
             <li>Lists make my life easier</li>
           </ul>
           <p>I also enjoy ordering things</p>
           <ol>
             <li>This is the first reason</li>
             <li>This is the second reason</li>
           </ol>
         </div>
         <ul id="scenarios" />
       </div>
       """

Scenario: Markdown Tables should be Formatted as HTML Tables

  Given I have this feature description
        """
        In order to see the description as nice HTML
        As a Pickles user
        I want to see the descriptions written in markdown rendered with tables

        | Table Header 1 | Table Header 2 |
        | -------------- | -------------- |
        | Cell value 1   | Cell value 2   |
        | Cell value 3   |                |
        | Cell value 4   | Cell value 5   |
        """
	When I generate the documentation
	Then the result should be
       """
       <div id="feature">
         <h1>a feature</h1>
         <div class="description">
           <p>In order to see the description as nice HTML
       As a Pickles user
       I want to see the descriptions written in markdown rendered with tables</p>
           <table>
             <thead>
               <tr>
                 <th>Table Header 1</th>
                 <th>Table Header 2</th>
               </tr>
             </thead>
             <tbody>
               <tr>
                 <td>Cell value 1</td>
                 <td>Cell value 2</td>
               </tr>
               <tr>
                 <td>Cell value 3</td>
                 <td></td>
               </tr>
               <tr>
                 <td>Cell value 4</td>
                 <td>Cell value 5</td>
               </tr>
             </tbody>
           </table>
         </div>
         <ul id="scenarios" />
       </div>
       """

Scenario: Somewhat malformed Markdown Tables should be formatted as HTML Tables as well

  Given I have this feature description
        """
        In order to see the description as nice HTML
        As a Pickles user
        I want to see the descriptions written in markdown rendered with tables

        | Table Header 1 | Table Header 2                         |
        | -------------- | -------------------------------------- |
        | Cell value 1   | Cell value 2                           |
        | Cell value 3     Note the missing column delimiter here |
        | Cell value 4   | Cell value 5                           |
        """
  When I generate the documentation
  Then the result should be
        """
        <div id="feature">
          <h1>a feature</h1>
          <div class="description">
            <p>In order to see the description as nice HTML
        As a Pickles user
        I want to see the descriptions written in markdown rendered with tables</p>
            <table>
              <thead>
                <tr>
                  <th>Table Header 1</th>
                  <th>Table Header 2</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>Cell value 1</td>
                  <td>Cell value 2</td>
                </tr>
                <tr>
                  <td>Cell value 3     Note the missing column delimiter here</td>
                  <td></td>
                </tr>
                <tr>
                  <td>Cell value 4</td>
                  <td>Cell value 5</td>
                </tr>
              </tbody>
            </table>
          </div>
          <ul id="scenarios" />
        </div>
        """