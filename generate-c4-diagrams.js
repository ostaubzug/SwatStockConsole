const fs = require('fs');
const path = require('path');
const https = require('https');

// Read the code context
const codeContext = JSON.parse(fs.readFileSync('code-context.json', 'utf8'));

// Function to call DeepSeek API
async function callDeepSeekAPI(prompt, outputFile, diagramType) {
  return new Promise((resolve, reject) => {
    const apiKey = process.env.DEEPSEEK_API_KEY;

    if (!apiKey) {
      reject(new Error('DEEPSEEK_API_KEY is not set'));
      return;
    }

    const data = JSON.stringify({
      model: "deepseek-reasoner",
      messages: [
        {
          role: "system", 
          content: "You are an expert software architect who specializes in creating C4 model diagrams using PlantUML syntax. Analyze the provided code context and generate accurate, well-structured diagrams that follow C4 model conventions."
        },
        { role: "user", content: prompt }
      ],
      temperature: 0.1,
      max_tokens: 4096
    });

    const options = {
      hostname: 'api.deepseek.com',
      path: '/chat/completions',
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${apiKey}`,
        'Content-Length': data.length
      }
    };

    const req = https.request(options, (res) => {
      let responseData = '';

      res.on('data', (chunk) => {
        responseData += chunk;
      });

      res.on('end', () => {
        try {
          const parsedResponse = JSON.parse(responseData);
          if (parsedResponse.error) {
            reject(new Error(`API error: ${parsedResponse.error.message}`));
            return;
          }

          const plantUmlContent = extractPlantUMLFromResponse(parsedResponse.choices[0].message.content);

          // Save the PlantUML content to the specified file
          fs.writeFileSync(outputFile, plantUmlContent);
          console.log(`Generated ${diagramType} diagram: ${outputFile}`);

          resolve(plantUmlContent);
        } catch (error) {
          reject(new Error(`Failed to parse API response: ${error.message}`));
        }
      });
    });

    req.on('error', (error) => {
      reject(new Error(`API request failed: ${error.message}`));
    });

    req.write(data);
    req.end();
  });
}

// Function to extract PlantUML code from the response
function extractPlantUMLFromResponse(response) {
  // Look for PlantUML code between @startuml and @enduml tags
  const match = response.match(/@startuml[\s\S]*?@enduml/);
  if (match) {
    return match[0];
  }

  // If no explicit tags, look for PlantUML-like content
  const lines = response.split('\n');
  let inCodeBlock = false;
  let plantUmlContent = [];
  let foundPlantUml = false;

  for (const line of lines) {
    if (line.trim().startsWith('```plantuml') || line.trim().startsWith('```puml')) {
      inCodeBlock = true;
      foundPlantUml = true;
      plantUmlContent.push('@startuml');
      continue;
    }

    if (inCodeBlock && line.trim() === '```') {
      inCodeBlock = false;
      plantUmlContent.push('@enduml');
      continue;
    }

    if (inCodeBlock) {
      plantUmlContent.push(line);
    }
  }

  if (foundPlantUml) {
    return plantUmlContent.join('\n');
  }

  // If still no PlantUML content found, wrap the entire response
  console.log("No PlantUML markers found, using response as raw content");
  return `@startuml\n${response}\n@enduml`;
}

// Generate context prompt from code context
function generateContextPrompt(diagramType) {
  let prompt = `Based on the following code context, generate a C4 ${diagramType} diagram using PlantUML syntax.\n\n`;
  prompt += `Code context:\n${JSON.stringify(codeContext, null, 2)}\n\n`;

  switch (diagramType) {
    case 'Context':
      prompt += "Create a System Context diagram showing the SwatStockConsole application, its users, and external dependencies like the Alpha Vantage API. Focus on high-level relationships between systems.";
      break;
    case 'Container':
      prompt += "Create a Container diagram showing the main components within the SwatStockConsole application. Include the API service, chart service, models, utilities, and console application containers and how they interact.";
      break;
    case 'Component':
      prompt += "Create a Component diagram detailing the internal components of the SwatStockConsole application, showing interfaces, implementations, and their relationships.";
      break;
    case 'Code':
      prompt += "Create a detailed Code diagram showing key classes, interfaces, and their relationships within the SwatStockConsole application.";
      break;
  }

  prompt += "\n\nUse the C4-PlantUML syntax and ensure your diagram follows C4 model best practices. Return only the PlantUML code without explanations.";

  return prompt;
}

// Main execution
async function generateAllDiagrams() {
  try {
    // Define the diagrams to generate
    const diagrams = [
      { type: 'Context', file: 'docs/c4/diagrams/c4_context.puml' },
      { type: 'Container', file: 'docs/c4/diagrams/c4_container.puml' },
      { type: 'Component', file: 'docs/c4/diagrams/c4_component.puml' }
    ];

    for (const diagram of diagrams) {
      const prompt = generateContextPrompt(diagram.type);
      await callDeepSeekAPI(prompt, diagram.file, diagram.type);
    }

    // Generate the AsciiDoc file
    generateAsciiDoc();

    console.log('All diagrams generated successfully');
  } catch (error) {
    console.error('Error generating diagrams:', error);
    process.exit(1);
  }
}

function generateAsciiDoc() {
  // Build AsciiDoc content using string concatenation to avoid YAML parsing issues
  let asciidocContent = "";
  asciidocContent += "= SwatStockConsole C4 Architecture Documentation\n\n";
  asciidocContent += ":toc: left\n";
  asciidocContent += ":toclevels: 3\n";
  asciidocContent += ":sectnums:\n";
  asciidocContent += ":plantuml-format: svg\n\n";

  asciidocContent += "== Introduction\n\n";
  asciidocContent += "This document provides the C4 model architecture documentation for the SwatStockConsole application.\n";
  asciidocContent += "The diagrams are automatically generated based on code analysis using DeepSeek AI.\n\n";

  asciidocContent += "== C4 Model\n\n";
  asciidocContent += "The C4 model is a simple way to document software architecture using four levels:\n\n";
  asciidocContent += "1. System Context: People and software systems\n";
  asciidocContent += "2. Containers: Applications and data stores\n";
  asciidocContent += "3. Components: Groups of related functionality\n";
  asciidocContent += "4. Code: Classes, interfaces, etc.\n\n";

  asciidocContent += "=== System Context Diagram\n\n";
  asciidocContent += "The System Context diagram shows the SwatStockConsole system in context with its users and external dependencies.\n\n";
  asciidocContent += "[plantuml, \"c4-context-diagram\", svg]\n";
  asciidocContent += "....\n";
  asciidocContent += "include::diagrams/c4_context.puml[]\n";
  asciidocContent += "....\n\n";

  asciidocContent += "=== Container Diagram\n\n";
  asciidocContent += "The Container diagram shows the major components within the SwatStockConsole system.\n\n";
  asciidocContent += "[plantuml, \"c4-container-diagram\", svg]\n";
  asciidocContent += "....\n";
  asciidocContent += "include::diagrams/c4_container.puml[]\n";
  asciidocContent += "....\n\n";

  asciidocContent += "=== Component Diagram\n\n";
  asciidocContent += "The Component diagram shows the internal components of the SwatStockConsole application.\n\n";
  asciidocContent += "[plantuml, \"c4-component-diagram\", svg]\n";
  asciidocContent += "....\n";
  asciidocContent += "include::diagrams/c4_component.puml[]\n";
  asciidocContent += "....\n\n";

  asciidocContent += "== Implementation Details\n\n";
  asciidocContent += "The SwatStockConsole application is a .NET console application that retrieves stock data from Alpha Vantage API\n";
  asciidocContent += "and displays it as candlestick charts in the console.\n\n";

  asciidocContent += "=== Key Components\n\n";
  asciidocContent += "- AlphaVantageApiService: Handles communication with the Alpha Vantage API\n";
  asciidocContent += "- ChartService: Renders ASCII-based candlestick charts\n";
  asciidocContent += "- JsonUtility: Parses JSON responses from the API\n";
  asciidocContent += "- Client: Orchestrates the application flow\n";
  asciidocContent += "- DailyPriceData: Data model for daily stock price information\n\n";

  asciidocContent += "== Maintenance\n\n";
  asciidocContent += "This documentation is automatically generated from the codebase and updated on every commit.\n";
  asciidocContent += "The diagrams are created using DeepSeek AI to analyze the code structure.\n\n";

  asciidocContent += "Last generated: " + new Date().toISOString();

  fs.writeFileSync('docs/c4/c4_documentation.adoc', asciidocContent);
  console.log('AsciiDoc file generated: docs/c4/c4_documentation.adoc');
}

// Start generation
generateAllDiagrams();
