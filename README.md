# Git 101

Install Git if you don't already have it.

Note: when you see `something like <_this_>`, which indicates code to type, copy it exactly EXCEPT for whatever is inside the `<>`. Replace the `<>` with something that makes sense, and don't include the actual brackets themselves.

## Make and submit changes

If you're just starting, refer to the "Create and link a local repository" section below to set up a repository. Then, whenever you work on some part of the project, navigate to your project folder in the terminal/command prompt and do the following:

1. Make sure you're up to date in your local repository.
    
    ```
    git checkout master
    git pull origin master
    ```

2. Create a new branch of the project. This allows you to isolate your changes from everyone else so you can work on a specific part (e.g. a certain class). When you finish, your branch is merged back into the master branch.
    
    `git branch <_branch name_>`
    
    Switch to your new branch with `git checkout <_branch name_>`

3. Write some code, blah blah blah. When you feel like you've reached a good checkpoint where things are working pretty well (not necessarily finished, but nothing will break), then:
    
    Run `git status` and look for any "Untracked files" in the output. If some files in the project are untracked, that means that Git will not include them when it updates. So, if there are any untracked files, run `git add -A .` to include all of them.
    
    Run `git commit -m '<_message_>'`. A commit is a sort of snapshot of the repository which we can compare to commits made at other points in time to see what's changed. The message should be short and simple but still descriptive of what you've changed, like "Added newclass.js" or "Added ability to do X" or "Made bots more difficult".

4. Submit your changes to your branch in the remote repository.
    
    `git push origin <_branch name_>`
    
    You should repeat steps 3 and 4 throughout the process of working on your part of the code. Save your code in the editor the most often, commit your changes locally when you finish some subsection of your assignment, and push to the remote at the end of the day.

5. In the repository on GitLab.com (where you are when you're reading this), make a new merge request ([tutorial](https://docs.gitlab.com/ee/gitlab-basics/add-merge-request.html)) to have your code reviewed and merged with the master.
    
    The source branch should be the branch you've been working on, and the target branch should be master. Assign the request to William Wu (Quaderatics) and ignore the milestones and labels.

6. If your merge request is accepted, switch back to master and delete your branch:
    
    ```
    git checkout master
    git branch -d <_branch name_>
    ```
    
    If there are still things you have to change, return to step 3 and repeat. You will not have to create a new merge request - just notify William when you're ready and push your changes again.

Summary:

1. `pull` from master
2. `branch` and `checkout`
3. `status` and `commit`
4. `push` to remote
5. merge request

## Create and link a local repository

You will need a Git repository on your computer to transfer changes/updates to and from this one online.

1. Create a new folder to contain all the project files. Make sure you can access it easily.

2. Navigate to this folder in the terminal/command prompt.
    MacOS and Windows: `cd <_filepath_>`

3. Enter the following commands:
    ```
    git init
    git remote add origin git@gitlab.com:Quaderatics/APCS-2018.git
    ```
